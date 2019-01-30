using System;

namespace BriLib
{
  public class SelectionContainer
  {
    private Type _selectableType;
    private ISelectable _current;

    public void Initialize(Type selectableType)
    {
      _selectableType = selectableType;
      MessageManager.Instance.Bus.Subscribe<SelectionMessage>(OnSelectionMessage);
    }

    public void Deinitialize()
    {
      MessageManager.Instance.Bus.Unsubscribe<SelectionMessage>(OnSelectionMessage);
    }

    private void OnSelectionMessage(SelectionMessage obj)
    {
      var selectableType = obj.NewSelection.GetType();
      if (selectableType != _selectableType || _current == obj.NewSelection) return;
      if (_current != null) _current.Selected.Value = false;
      obj.NewSelection.Selected.Value = true;
      _current = obj.NewSelection;
    }
  }
}
