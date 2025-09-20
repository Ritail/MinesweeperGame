using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

public class CellHandler : MonoBehaviour, IPointerClickHandler
{
   public Vector2Int Position;
   public TextMeshProUGUI Text;

   private bool _isReveal;
   private GameManager _gameManager;

  
   public void Setup(Vector2Int position, GameManager gameManager)
   {
      Text.GetComponent<TextMeshPro>();
      Position = position;
      _gameManager = gameManager;
   }

   public void Reveal(int value)
   {
      _isReveal = true;
      Text.text = value.ToString();
   }

   public void OnPointerClick(PointerEventData eventData)
   {
      _gameManager.CellReveal(Position);
   }
}
