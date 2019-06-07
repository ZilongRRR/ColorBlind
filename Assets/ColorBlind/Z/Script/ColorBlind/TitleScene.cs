using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
namespace ZTools {
    public class TitleScene : MonoBehaviour {
        public TextMeshProUGUI titleText;
        public string titleString;
        public ColorData colorData;
        int cl;
        // Start is called before the first frame update
        void Start () {
            cl = colorData.colorChips.Length;
            StartCoroutine (RandomTitleColor ());
        }
        IEnumerator RandomTitleColor () {
            titleText.text = "";
            var charArray = titleString.ToCharArray ();
            int ci = Random.Range (0, cl);
            for (int i = 0; i < charArray.Length; i++) {
                var hex = ColorUtility.ToHtmlStringRGB (colorData.colorChips[ci]);
                titleText.text += "<color=#" + hex + ">" + charArray[i].ToString () + "</color>";
                ci = (ci + 1) % cl;
            }
            yield return new WaitForSeconds (2);
            StartCoroutine (RandomTitleColor ());
        }
    }
}