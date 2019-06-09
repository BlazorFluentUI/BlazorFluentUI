using System;
using System.Collections.Generic;
using System.Text;

namespace BlazorFabric.BaseComponent
{
    public class PositionDirectionalHintData
    {
        public RectangleEdge TargetEdge { get; set; }
        public RectangleEdge AlignmentEdge { get; set; }
        public bool IsAuto { get; set; }
        public bool AlignTargetEdge { get; set; }

        public PositionDirectionalHintData(RectangleEdge targetEdge, RectangleEdge alignmentEdge, bool isAuto = false)
        {
            this.TargetEdge = targetEdge;
            this.AlignmentEdge = alignmentEdge;
            this.IsAuto = isAuto;
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

    public class CalloutPositionedInfo : ElementPosition
    {
        public CalloutPositionedInfo(ElementPosition elementPosition, ElementPosition beakPosition): base(elementPosition.ElementRectangle, elementPosition.TargetEdge, elementPosition.AlignmentEdge)
        {
            BeakPosition = beakPosition;
        }

        public ElementPosition BeakPosition { get; set; }
    }

    public enum RectangleEdge
    {
        None = 0,
        Top = 1,
        Bottom = -1,
        Left = 2,
        Right = -2
    }

    public enum Position
    {
        Top = 0,
        Bottom = 1,
        Start = 2,
        End = 3
    }
}
