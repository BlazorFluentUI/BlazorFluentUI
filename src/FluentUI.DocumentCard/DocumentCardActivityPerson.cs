namespace FluentUI
{
    public class DocumentCardActivityPerson
    {
        /// <summary>
        ///  The name of the person.
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// Path to the profile photo of the person.
        /// </summary>
        public string? ProfileImageSrc { get; set; }
        /// <summary>
        ///  The user's initials to display in the profile photo area when there is no image.
        /// </summary>
        public string? Initials { get; set; }
        /// <summary>
        /// Whether initials are calculated for phone numbers and number sequences.
        /// Example: Set property to true to get initials for project names consisting of numbers only.
        /// Default false
        /// </summary>
        public bool AllowPhoneInitials { get; set; }

        /// <summary>
        ///  The background color when the user's initials are displayed. Default PersonaInitialsColor.Blue
        /// </summary>
        public PersonaInitialsColor? InitialsColor { get; set; }

        public DocumentCardActivityPerson()
        {
            InitialsColor = PersonaInitialsColor.Blue;
        }
    }
}