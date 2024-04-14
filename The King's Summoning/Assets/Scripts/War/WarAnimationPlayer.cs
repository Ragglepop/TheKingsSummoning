using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarAnimationPlayer : MonoBehaviour
{
    public TextFieldController tController;
    public float AnimationDelay;
    public Animator anim;
    private static readonly int Fight1 = Animator.StringToHash("Fight1");
    private static readonly int Fight2 = Animator.StringToHash("Fight2");
    private static readonly int Fight3 = Animator.StringToHash("Fight3");
    private static readonly int Lose = Animator.StringToHash("Lose");

    // Start is called before the first frame update
    void Start()
    {
        TextFieldController.OnPause+=PlayAnimation;
        TextFieldController.OnLose+=PlayLoseAnimation;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void PlayAnimation(){
        StartCoroutine(PlayAnimationThenUnpause());
    }

    private IEnumerator PlayAnimationThenUnpause(){
        if(tController.wordIndex<3){
            anim.SetTrigger(Fight1);
        }else if(tController.wordIndex<5){
            anim.SetTrigger(Fight2);
        }else{
            anim.SetTrigger(Fight3);
        }
        
        yield return new WaitForSeconds(AnimationDelay);
        tController.PlayGame();
    }
    private void PlayLoseAnimation(){
        anim.SetTrigger(Lose);
    }
}
