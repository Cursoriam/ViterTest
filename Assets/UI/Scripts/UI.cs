using UnityEngine;
using UnityEngine.UI;

public class UI : MonoBehaviour
{
    [SerializeField] private Text label;
    [SerializeField] private Animator animator;
    
    public void ActivateWinGameWindow()
    {
        label.text = Constants.WinGameText;
    }

    public void ActivateLoseGameWindow()
    {
        label.text = Constants.LoseGameText;
    }

    public void ActivateAnimation()
    {
        animator.enabled = true;
    }
}
