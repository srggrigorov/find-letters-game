using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class QuizManager : MonoBehaviour
{
    [System.Serializable]
    public struct UsedData
    {
        public TextAsset answers;
        public SpriteAtlas spriteAtlas;
        public string question;

    }

    public UsedData[] typesOfData;
    [Space(40)]
    public GameObject cellPrefab;
    private RectTransform cellRectTransform;
    public Transform canvas;
    private Sprite[] _cellsSprites;
    public Sprite[] CellsSprites { get => _cellsSprites; }
    private string[] _answers;
    public string[] Answers { get => _answers; }
    private string _question;
    private Cell[,] _cells;
    private int _level = 0;
    private string _rightAnswer;
    public string RightAnswer { get => _rightAnswer; }
    public GameObject questionField;
    private List<string> _usedAnswers = new List<string>();
    public Button restartButton;
    public Image fadeScreen;








    private void Start()
    {
        cellRectTransform = cellPrefab.GetComponent<RectTransform>();
        Sequence startSequence = DOTween.Sequence();
        startSequence.Append(fadeScreen.DOFade(0, 0.5f));
        startSequence.OnComplete(StartNewLevel);

    }



    private void ChooseData()
    {
        UsedData usedData = typesOfData[Random.Range(0, typesOfData.Length)];
        _answers = usedData.answers.text.ToUpper().Split(new[] { "\r\n", "\r", "\n", " " }, System.StringSplitOptions.RemoveEmptyEntries);
        _cellsSprites = new Sprite[usedData.spriteAtlas.spriteCount];
        _question = usedData.question;
        usedData.spriteAtlas.GetSprites(_cellsSprites);
    }


    private void CreateCells(int level)
    {

        _cells = new Cell[3, level];
        int i = 0;
        int j = 0;
        List<string> usedContent = new List<string>();

        for (float y = cellRectTransform.sizeDelta.y; y >= -cellRectTransform.sizeDelta.y * level; y -= cellRectTransform.sizeDelta.y)
        {
            for (float x = -cellRectTransform.sizeDelta.x; x <= cellRectTransform.sizeDelta.x; x += cellRectTransform.sizeDelta.x)
            {
                _cells[i, j] = Instantiate(cellPrefab, transform.position, Quaternion.identity).GetComponent<Cell>();
                _cells[i, j].gameObject.transform.SetParent(canvas.transform, false);
                _cells[i, j].transform.localPosition = Vector3.zero;
                _cells[i, j].RectTransform.anchoredPosition = new Vector3(x, y, 0);
                string cellContent = _answers[Random.Range(0, _answers.Length)];

                do cellContent = _answers[Random.Range(0, _answers.Length)];
                while (usedContent.Contains(cellContent));

                usedContent.Add(cellContent);
                _cells[i, j].Init(cellContent, this);
                if (_level == 1)
                {
                    _cells[i, j].Bounce();
                }

                if (i == 2)
                {
                    j++;
                    i = 0;
                }
                else i++;
                if (j == level)
                {

                    do _rightAnswer = _cells[Random.Range(0, 3), Random.Range(0, level)].Content;
                    while (_usedAnswers.Contains(_rightAnswer));

                    usedContent.Clear();
                    return;
                }
            }
        }

    }

    private void ShowQuestion()
    {
        if (!questionField.activeInHierarchy)
        {
            questionField.SetActive(true);

        }
        TextMeshProUGUI textMesh = questionField.GetComponentInChildren<TextMeshProUGUI>();
        textMesh.SetText(_question + " " + _rightAnswer);
        if (_level == 1)
        {
            questionField.GetComponentInChildren<Image>().DOFade(145, 1f);
            textMesh.DOFade(255, 3f);

        }

    }

    public void StartNewLevel()
    {
        _level++;
        Cell[] allCells = FindObjectsOfType<Cell>();
        if (allCells != null)
        {
            for (int i = 0; i < allCells.Length; i++)
            {
                Destroy(allCells[i].gameObject);
            }
        }
        if (_level > 3)
        {
            questionField.SetActive(false);
            restartButton.gameObject.SetActive(true);
            Sequence endSequence = DOTween.Sequence();
            endSequence.Append(fadeScreen.DOFade(255, 2f));

            restartButton.GetComponent<Image>().DOFade(255, 1f);

            restartButton.onClick.AddListener(TaskOnRestartClick);

        }
        else
        {
            if (_rightAnswer != null)
                _usedAnswers.Add(_rightAnswer);

            ChooseData();
            CreateCells(_level);
            ShowQuestion();

        }

    }

    private void TaskOnRestartClick()
    {
        _level = 0;
        Sequence sequence = DOTween.Sequence();
        sequence.Append(fadeScreen.DOFade(255, 1f));
        sequence.Append(restartButton.GetComponent<Image>().DOFade(0, 2f));
        sequence.Join(fadeScreen.DOFade(0, 0.5f));
        restartButton.gameObject.SetActive(false);
        sequence.OnComplete(StartNewLevel);
    }






}
