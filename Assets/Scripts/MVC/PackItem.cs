using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PackItem : MonoBehaviour {
    private Image goodIcon;
    private Text countText;

	
	void Awake () {
        goodIcon = transform.Find("Image").GetComponent<Image>();
        countText = transform.Find("Text").GetComponent<Text>();
	}

    public void SetModel(BasePackage pack) {
        if (pack.Count == 0)
        {
            goodIcon.enabled = false;
            countText.text = string.Empty;
        }
        else {
            goodIcon.enabled = true;
            goodIcon.sprite = Resources.Load<Sprite>(pack.item.IconPath);
            countText.text = pack.Count.ToString();
        }
    }
}
