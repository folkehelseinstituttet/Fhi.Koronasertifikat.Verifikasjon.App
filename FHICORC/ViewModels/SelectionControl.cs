namespace FHICORC.ViewModels
{
    public class SelectionControl : ISelection
    {
        public virtual bool IsSelected { get; set; }
        public virtual string Text { get; set; }
        public virtual string AccessibilityText => IsSelected
            ? Text + "Selected"
            : Text;

        public override string ToString()
        {
            return Text;
        }
    }
}
