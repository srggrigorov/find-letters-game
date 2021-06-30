using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Cell : MonoBehaviour
{
    private RectTransform _rectTransform;
    private Button _button;
    public RectTransform RectTransform { get => _rectTransform; set => _rectTransform = value; }
    private QuizManager _quizManager;
    private Sprite _cellSprite;
    public Gradient gradient;
    private string _content;
    public string Content
    {
        get { return _content; }
    }
    public ParticleSystem _fireworks;


    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
        _button = GetComponent<Button>();
        _button.onClick.AddListener(TaskOnClick);
        _fireworks.transform.SetParent(transform);



    }

    public void Init(string cellContent, QuizManager quizManager)
    {
        _content = cellContent;
        _quizManager = quizManager;
        for (int i = 0; i < _quizManager.CellsSprites.Length; i++)
        {
            if (_quizManager.CellsSprites[i].name == _content + "(Clone)")
            {
                _cellSprite = _quizManager.CellsSprites[i];
                break;
            }
        }
        if (_cellSprite != null)
        {
            Image spriteImage = transform.Find("Sprite").GetComponent<Image>();
            spriteImage.sprite = _cellSprite;
            spriteImage.SetNativeSize();
        }
        transform.Find("Cell Color").GetComponent<Image>().color = gradient.Evaluate(Random.Range(0.0f, 1.0f));

    }

    void TaskOnClick()
    {
        if (Content == _quizManager.RightAnswer)
        {
            _fireworks.Play();
            RectTransform contentTransform = transform.Find("Sprite").GetComponent<RectTransform>();
            Sequence sequence = DOTween.Sequence();
            sequence.Append(contentTransform.DOPunchScale(contentTransform.localScale * 1.2f, 0.3f, 10, 2));
            sequence.OnComplete(_quizManager.StartNewLevel);

        }
        else
        {
            RectTransform.DOShakePosition(0.5f, 10, 15, 90, false, true)
            .SetEase(Ease.InOutBounce);

        }
    }

    public void Bounce()
    {
        Vector3 startScale = RectTransform.localScale;
        RectTransform.localScale = Vector3.one * 0.01f;
        Sequence sequence = DOTween.Sequence();
        sequence.Append(RectTransform.DOScale(startScale * 1.2f, 0.4f));
        sequence.Append(RectTransform.DOScale(startScale * 0.7f, 0.3f));
        sequence.Append(RectTransform.DOScale(startScale * 1.1f, 0.2f));
        sequence.Append(RectTransform.DOScale(startScale * 0.9f, 0.1f));
        sequence.Append(RectTransform.DOScale(startScale, 0.1f));



    }

}
