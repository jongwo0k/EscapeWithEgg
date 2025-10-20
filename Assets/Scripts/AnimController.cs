using UnityEngine;

public class AnimController : MonoBehaviour
{
    private Animator anim;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        anim = GetComponent<Animator>();

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // 애니메이션 재생
    private void AnimatorChange(string temp)
    {
        anim.SetBool("isIDLE", false);
        anim.SetBool("isRun", false);
        if (temp == "isJump")
        {
            anim.SetTrigger(temp);
            return;
        }
        if (temp == "isDash")
        {
            anim.SetTrigger(temp);
            return;
        }
        anim.SetBool(temp, true);
    }
}
