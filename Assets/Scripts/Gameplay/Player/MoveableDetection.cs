using Comma.Gameplay.DetectableObject;
using Comma.Global.AudioManager;
using UnityEngine;
using Comma.Global.PubSub;
using Comma.Utility.Collections;

namespace Comma.Gameplay.Player
{
    [RequireComponent(typeof(Collider2D))]
    public class MoveableDetection : MonoBehaviour, IDebugger
    {
        [SerializeField] private Collider2D _rightArm;
        [SerializeField] private Collider2D _leftArm;
        //private PlayerMovement _player;
        private PEnhanceMovement _enhanceMovement;
        //private PlayerAnimationController _playerAnimator;
        private Rigidbody2D _rigid;

        private void Awake()
        {
            //_playerAnimator = GetComponent<PlayerAnimationController>();
            //_player = GetComponent<PlayerMovement>();
            _enhanceMovement = GetComponent<PEnhanceMovement>();
            _rigid= GetComponent<Rigidbody2D>();
        }
        private void Start()
        {
            
            EventConnector.Subscribe("OnPlayerInteract", new(OnInteract));
            EnableArms(false, false);

        }
        private void EnableArms(bool condition = false, bool complete = true)
        {
            _rightArm.isTrigger = !condition;
            _leftArm.isTrigger = !condition;
            //if (!complete) return;
            _rightArm.enabled = complete;
            _leftArm.enabled = complete;
            //_rightArm.SetActive(condition);
            //_leftArm.SetActive(condition);
        }

        #region PubSub
        private bool _isHoldMoveable;
        private IMoveableObject _moveableObject;
        private bool _objectOnRight;
        private float _direction;
        private void OnInteract(object msg)
        {
            

            if (_isHoldMoveable)
            {
                EjectMovable();
                EnableArms(condition: false);
            }
            else if (_moveableObject != null)
            {
                if (!_inGrabArea) return;
                _isHoldMoveable = true;
                //_player.IsFlipProhibited = true;
                _enhanceMovement.IsFlipProhibited = true;
                HandleMovable(1);
                _moveableObject.Interact();
                //_playerAnimator.Push = true;
                Holded = true;
                Push = true;
                SfxPlayer.Instance.PlaySFX("InteractObject", true);
                EnableArms(condition: true);
                //SFXController.Instance.PlayInteractObjectSFX();
            }
        }
        private void EjectMovable()
        {
            _isHoldMoveable = false;
            //_player.IsFlipProhibited = false;
            _enhanceMovement.IsFlipProhibited = false;
            //_playerAnimator.Push = false;
            //_playerAnimator.Pull = false;
            Holded = false;
            Push = false;
            _moveableObject.UnInteract();
            EnableArms(false, false);
            //SFXController.Instance.StopObjectSFX();
        }
        #endregion

        #region Detection
        private bool _inGrabArea;
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Moveable"))
            {
                Vector2 normal = collision.transform.position - transform.position;
                _inGrabArea = true;
                _moveableObject = collision.GetComponent<IMoveableObject>();

                _objectOnRight = normal.x > 0;
            }
        }
        //private void OnTriggerExit2D(Collider2D collision)
        //{
        //    if (collision.CompareTag("Moveable"))
        //    {
        //        _inGrabArea = false;
        //        if (_isHoldMoveable)
        //        {
        //            EjectMovable();
        //        }
        //    }
        //}
        public bool Push { get; set; }
        public bool Holded { get; set; }
        private void ChangeAnimationState()
        {
            if (!_isHoldMoveable) return;
            //_direction = _player.GetInput;
            _direction = _enhanceMovement.HorizontalInput;
            int normalizeDirection = (int) _direction * (_objectOnRight ? 1 : -1);
            PlayerFlip(_objectOnRight ? 1 : -1);


            // Play SFX
            switch (normalizeDirection)
            {
                case 0:
                    //SFXController.Instance.StopObjectSFX();
                    if (!_isHoldMoveable)
                    {
                        //_playerAnimator.Push = false; 
                        //_playerAnimator.Pull = false;
                        Holded = false;
                    }
                    break;
                case 1:
                    //SFXController.Instance.PlayPushSFX();
                    //_playerAnimator.Push = true;
                    //_playerAnimator.Pull = false;
                    Holded = true;
                    Push = true;
                    break;
                case -1:
                    //SFXController.Instance.PlayPullSFX();
                    //_playerAnimator.Push = false;
                    //_playerAnimator.Pull = true;
                    Holded = true;
                    Push = false;
                    break;
            }

        } 
        private void HandleMovable(float direction)
        {
            if (!_isHoldMoveable) return;
            _moveableObject.GetDetection(_rigid, direction);
        }
        #endregion

        private void Update()
        {
            ChangeAnimationState();
        }



        private void PlayerFlip(float dir)
        {
            if (dir > 0)
            {
                GetComponent<SpriteRenderer>().flipX = false;
            }
            else if(dir < 0)
            {
                GetComponent<SpriteRenderer>().flipX = true;
            }
        }


        public string ToDebug()
        {
            string returner = "\n";
            returner += $"In Grab Area: <i>{_inGrabArea}</i>\n";
            returner += $"Is interacting: <i>{_isHoldMoveable}</i>\n";
            returner += $"IMovable: <i>{_moveableObject}</i>\n";
            returner += $"From Right: <i>{_objectOnRight}</i>\n";

            return returner;
        }
    }
}