namespace BlazorFluentUI
{
    public class PositionDirectionalHintData
    {
        public RectangleEdge TargetEdge { get; set; }
        public RectangleEdge AlignmentEdge { get; set; }
        public bool IsAuto { get; set; }
        public bool AlignTargetEdge { get; set; }

        public PositionDirectionalHintData(RectangleEdge targetEdge, RectangleEdge alignmentEdge, bool isAuto = false)
        {
            TargetEdge = targetEdge;
            AlignmentEdge = alignmentEdge;
            IsAuto = isAuto;
        }
    }

    public class RelativePositions
    {
        public Position CalloutPosition { get; set; }
        public Position BeakPosition { get; set; }
        public string DirectionalClassName { get; set; }
        public DirectionalHint SubmenuDirection { get; set; }

        public RelativePositions(Position position, Position beakPosition, string dirClassName, DirectionalHint submenuDir)
        {
            CalloutPosition = position;
            BeakPosition = beakPosition;
            DirectionalClassName = dirClassName;
            SubmenuDirection = submenuDir;
        }
    }

    public class ElementPosition
    {
        public Rectangle ElementRectangle { get; set; }
        public RectangleEdge TargetEdge { get; set; }
        public RectangleEdge AlignmentEdge { get; set; }

        public ElementPosition(Rectangle elementRectangle, RectangleEdge targetEdge, RectangleEdge alignmentEdge = RectangleEdge.None)
        {
            ElementRectangle = elementRectangle;
            TargetEdge = targetEdge;
            AlignmentEdge = alignmentEdge;
        }
        
    }

    public static class ElementPositionEx
    {
        public static ElementPositionInfo ToElementPositionInfo(this ElementPosition elementPosition, Rectangle targetRectangle)
        {
            return new ElementPositionInfo(elementPosition, targetRectangle);
        }
    }

    public class ElementPositionInfo : ElementPosition
    {
        public Rectangle TargetRectangle { get; set; }

        public ElementPositionInfo(ElementPosition elementPosition, Rectangle targetRectangle):base(elementPosition.ElementRectangle, elementPosition.TargetEdge,elementPosition.AlignmentEdge)
        {
            TargetRectangle = targetRectangle;
        }
    }

   

    public enum RectangleEdge
    {
        None = 0,
        Top = 1,
        Bottom = -1,
        Left = 2,
        Right = -2
    }

    public class Position
    {
        public int? Top { get; set; }
        public int? Left { get; set; }
        public int? Bottom { get; set; }
        public int? Right { get; set; }
        public string? Key { get; set; }
    }

    public class PositionProperties
    {
        public object? Target { get; set; }
        public DirectionalHint? DirectionalHint { get; set; }
        public DirectionalHint? DirectionalHintForRTL { get; set; }

        public int? GapSpace { get; set; }
        public Rectangle? Bounds { get; set; }
        public bool? CoverTarget { get; set;  }
        public bool? DirectionalHintFixed { get; set; }

        public bool? AlignTargetEdge { get; set; }
    }

    public class CalloutPositionProperties : PositionProperties
    {
        public int? BeakWidth {  get; set;  }
        public bool? IsBeakVisible { get; set; }
    }

    public class PositionedData
    {
        public Position ElementPosition {  get; set;  }
        public RectangleEdge TargetEdge { get; set;  }

        public RectangleEdge? AlignmentEdge { get; set; } 
    }

    public class CalloutPositionedInfo : PositionedData
    {
        

        public PartialRectangle ElementRectangle { get; set; }


        public CalloutBeakPositionedInfo BeakPosition { get; set; }

        public CalloutPositionedInfo()
        {
            BeakPosition = new CalloutBeakPositionedInfo();
            ElementRectangle = new PartialRectangle();
        }
        public CalloutPositionedInfo(PartialRectangle rectangle, Position? position, RectangleEdge targetEdge, RectangleEdge alignmentEdge, CalloutBeakPositionedInfo beakPosition)
        {
            ElementRectangle = rectangle;
            ElementPosition = position;
            TargetEdge = targetEdge;
            AlignmentEdge = alignmentEdge;
            BeakPosition = beakPosition;
        }
    }

    public class CalloutBeakPositionedInfo : PositionedData
    {
        public PartialRectangle ElementRectangle { get; set; }

        public RectangleEdge ClosestEdge { get; set; }

        public CalloutBeakPositionedInfo()
        {
            ElementRectangle = new PartialRectangle();
        }
        public CalloutBeakPositionedInfo(PartialRectangle elementRectangle, Position position, RectangleEdge targetEdge, RectangleEdge closestEdge)
        {
            ElementRectangle = elementRectangle;
            ElementPosition = position;
            TargetEdge = targetEdge;
            AlignmentEdge = RectangleEdge.None;
            ClosestEdge = closestEdge;
        }
    }
}
