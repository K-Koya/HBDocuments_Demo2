using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sequencer : MonoBehaviour
{
    List<IEnumerator> _iEnums = new List<IEnumerator>(10);

    // Start is called before the first frame update
    void Start()
    {
        StartSequence(ExecuteAsync());
    }

    // Update is called once per frame
    void Update()
    {
        foreach (IEnumerator iEnum in _iEnums)
        {
            if (!iEnum.MoveNext())
            {
                _iEnums.Remove(iEnum);
            }
        }
    }

    public void StartSequence(IEnumerator func)
    {
        _iEnums.Add(func);
    }

    public void StopSequence(IEnumerator func)
    {
        if (_iEnums.Remove(func))
        {
            func.Reset();
        }
        else
        {
            Debug.LogWarning($"ƒV[ƒPƒ“ƒX‚É“o˜^‚³‚ê‚Ä‚¢‚È‚¢ƒƒ\ƒbƒh‚ğ’â~‚µ‚æ‚¤‚Æ‚µ‚Ü‚µ‚½B");
        }
    }



    private IEnumerator ExecuteAsync()
    {
        while (true)
        {
            var t = 0f;

            // X²‚Ì‰ñ“]‚ğ2•bŠÔ
            yield return RotateAsync(Vector3.right, 2);

            // 1•bŠÔ‘Ò‹@‚·‚é
            yield return WaitSeconds(1);

            // Y²‚Ì‰ñ“]‚ğ2•bŠÔ
            yield return RotateAsync(Vector3.up, 2);

            // 1•bŠÔ‘Ò‹@‚·‚é
            yield return WaitSeconds(1);

            // Z²‚Ì‰ñ“]‚ğ2•bŠÔ
            yield return RotateAsync(Vector3.forward, 2);

            // 1•bŠÔ‘Ò‹@‚·‚é
            yield return WaitSeconds(1);
        }
    }

    IEnumerator RotateAsync(Vector3 eulers, float time)
    {
        float t = 0f;
        while (t < time)
        {
            t += Time.deltaTime;
            transform.Rotate(eulers);
            yield return null;
        }
    }

    IEnumerator WhenAny(params YieldInstruction[] iEnums)
    {
        yield return null;
    }

    IEnumerator WaitSeconds(float sec)
    {
        float t = 0f;
        while (t < sec)
        {
            t += Time.deltaTime;
            yield return null;
        }
    }

    IEnumerator WaitMouseLeftClick()
    {
        while (!Input.GetMouseButtonDown(0))
        {
            yield return null; 
        }
    }
}
