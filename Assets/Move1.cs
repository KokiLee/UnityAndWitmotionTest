using UnityEngine;
using UnityEngine.InputSystem;

public class Cat : MonoBehaviour
{
    private Animator animator = null;
    private Vector2 move = Vector2.zero;

    public void Awake()
    {
        this.animator = this.GetComponent<Animator>();
    }

    public void Update()
    {
        if (this.move != Vector2.zero)
        {
            this.transform.Translate(this.move * Time.deltaTime * 3f);
        }
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        // キー入力が離れた場合も更新される
        this.move = context.ReadValue<Vector2>();
        var normalized = new Vector3(Mathf.Round(move.normalized.x), Mathf.Round(move.normalized.y), 0);

        // 斜め方向も許容
        if (normalized != Vector3.zero)
        {
            this.animator.SetFloat("x", normalized.x);
            this.animator.SetFloat("y", normalized.y);
        }
    }
}