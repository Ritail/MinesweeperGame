using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class CellHandler : MonoBehaviour
{
   private Vector2Int _position;
   public TextMeshProUGUI Text;

  
   public void Setup(Vector2Int position, string text)
   {
      Text.GetComponent<TextMeshPro>();
      _position = position;
      Text.text = text;
   }
}
