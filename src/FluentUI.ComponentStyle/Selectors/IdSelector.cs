namespace FluentUI
{
    public class IdSelector : ISelector
    {
        public string SelectorName { get; set; }

        public PseudoElements PseudoElement { get; set; } = PseudoElements.None;

        public PseudoClass PseudoClass { get; set; }


        private string ToPseudoClass()
        {
            if(PseudoClass == null || PseudoClass.PseudoClassType == PseudoClasses.None)
                return "";
            if(PseudoClass.PseudoClassType == PseudoClasses.NthChild 
                || PseudoClass.PseudoClassType == PseudoClasses.NthLastChild
                || PseudoClass.PseudoClassType == PseudoClasses.NthOfType
                || PseudoClass.PseudoClassType == PseudoClasses.NthLastOfType
                || PseudoClass.PseudoClassType == PseudoClasses.Lang
                || PseudoClass.PseudoClassType == PseudoClasses.Not
                || PseudoClass.PseudoClassType == PseudoClasses.Matches)
                return $"{PseudoMapper.PseudoClassesMappper[PseudoClass.PseudoClassType]}({PseudoClass.Value})";
            return $"{PseudoMapper.PseudoClassesMappper[PseudoClass.PseudoClassType]}";
        }

        public string GetSelectorAsString()
        {
            return $"#{(SelectorName != null ? SelectorName : "")}{(PseudoElement != PseudoElements.None ? PseudoMapper.PseudoElementsMappper[PseudoElement] : "")}{ToPseudoClass()}";
        }

    }
}