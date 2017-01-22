using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public class LevelCellButtonHandler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Level.NamedPrefab[] CellsToChoseFrom;

    public string Name;
    public int rotation;

    private int currentElement = 0;
    private GameObject currentCell;
    private Image image;
    private GameObject buttons;
    private Vector3 _rotation;

    void Awake()
    {
        image = transform.Find("LevelImage").gameObject.GetComponent<Image>();
        buttons = transform.Find("Buttons").gameObject;
        DeactivateButtons();
    }

	// Use this for initialization
	void Start () {
        SetCellAndImage();
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    public void ActivateButtons()
    {
        buttons.SetActive(true);
    }

    public void DeactivateButtons()
    {
        buttons.SetActive(false);
    }

    public void Next()
    {
        ++currentElement;
        currentElement = currentElement%CellsToChoseFrom.Length;
        SetCellAndImage();
    }

    public void Previous()
    {
        if (--currentElement < 0) currentElement = CellsToChoseFrom.Length - 1;
        SetCellAndImage();
    }

    public void Rotate()
    {
        _rotation = image.transform.eulerAngles;
        _rotation.z -= 90;
        image.transform.eulerAngles = _rotation;
        rotation = Mathf.RoundToInt(_rotation.z)%360;
    }

    private void SetImage()
    {
        image.sprite = currentCell.GetComponent<SpriteRenderer>().sprite;
    }

    private void SetCell()
    {
        currentCell = CellsToChoseFrom[currentElement].prefab;
        Name = CellsToChoseFrom[currentElement].name;
    }

    private void SetCellAndImage()
    {
        SetCell();
        SetImage();
    }


    public void OnPointerExit(PointerEventData eventData)
    {
        DeactivateButtons();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        ActivateButtons();
    }
}
