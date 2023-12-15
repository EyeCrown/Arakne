using UnityEngine;
using UnityEngine.Events;

public class States : MonoBehaviour
{
    [SerializeField] int etat = 0;
    [SerializeField] SpriteRenderer render_1;
    [SerializeField] SpriteRenderer render_2;
    [SerializeField] TrailRenderer trail;
    [SerializeField] ParticleSystem particle;

    [SerializeField] Gradient throwGradient;
    [SerializeField] Gradient passGradient;
    [SerializeField] Gradient fallGradient;

    [SerializeField] BouncingBallScript ball;
    public UnityEvent UpdateColor;


    // Start is called before the first frame update
    void Start()
    {
        UpdateColor.AddListener(UpdateColorHandler);

    }

    // Update is called once per frame
    void Update()
    {

    }
    void ChangeColor(Color color)
    {
        render_1.material.EnableKeyword("_EMISSION");
        render_1.material.SetColor("_EmisColor", color);
        render_2.material.SetColor("_EmisColor", color);
        trail.startColor = color;
        particle.startColor = color;
    }

    void UpdateColorHandler()
    {
        Color color;
        switch(ball.mode) {
            case BouncingBallScript.BallMode.bouncing:
                ChangeColor(GetColor(throwGradient));
                break;
            case BouncingBallScript.BallMode.homing:
                ChangeColor(GetColor(passGradient));
                break;
            case BouncingBallScript.BallMode.fall:
                ChangeColor(GetColor(fallGradient));
                break;
            default:
                break;
        }
    }

    private Color GetColor(Gradient gradient)
    {
        return gradient.Evaluate((float)ball.power/(float)ball.maxPower);
    }
}
