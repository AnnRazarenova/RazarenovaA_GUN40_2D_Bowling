//using UnityEngine;

//public class ScoreCount : MonoBehaviour
//{
//    [SerializeField]
//    private float timeWait = 3f;

//    private float lastHitTime = 0;

//    private int _score = 0;
//    private int _totalScore = 0;

//    private bool _isHitted = false;

//    public bool IsHitted {  get { return _isHitted; } }

//    private void OnTriggerEnter(Collider other)
//    {
//        if(other.gameObject.layer == 3 || other.gameObject.layer == 6)
//        {
//            if(!_isHitted)
//            {
//                _isHitted = true;
//                _score++;

//                CountPlayerScore(_score);
//            }
//        }
//    }



//    private void CountPlayerScore(int score)
//    {
//        _totalScore += score;
//    }    
//}
