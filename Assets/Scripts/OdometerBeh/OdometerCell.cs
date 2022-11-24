using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class OdometerCell : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI textMesh;

    const float NUMERIC_OFFSET_MULTIPLIER = 36.0f;
    const float TURN_SPEED = 0.25f;

    private int cur_value;

    private float oldTweenValue;

    public void SetCell(int value)
    {
        float from, to;

        int a = (cur_value + value) % 10;
        int b = (cur_value - value) % 10;

        from = NUMERIC_OFFSET_MULTIPLIER * (cur_value % 10);
        to = NUMERIC_OFFSET_MULTIPLIER * (value % 10);

        StartCoroutine(TweenUpdate(new float[] { from, to }));

        cur_value = value;
    }

    IEnumerator TweenUpdate(float[] param)
    {
        float from = param[0];
        float to;
        to = param[1] - param[0];

        float start_time = Time.time;
        float end_time = TURN_SPEED;
        float val = 0;

        while (Time.time - start_time <= end_time)
        {
            float t = Time.time;
            val = getTweenValue(Time.time - start_time, from, to, end_time);

            if (val != 0 && oldTweenValue != val)
            {
                var pos = textMesh.GetComponent<RectTransform>();
                var tween = pos.DOLocalMoveY(100, 0.3f).OnComplete(() =>
                {
                    pos.position = new Vector2(pos.position.x, -100);
                    textMesh.text = (param[1] / NUMERIC_OFFSET_MULTIPLIER).ToString();
                });

                yield return tween.WaitForCompletion();

                pos.DOLocalMoveY(0, 0.3f);
                oldTweenValue = val;
            }


            yield return null;
        }
    }

    float getTweenValue(float t_curtime, float b_startval, float c_amounttotal, float d_totaltime)
    {
        return (c_amounttotal * (t_curtime / d_totaltime)) + b_startval;
    }

}
