namespace FieldBank.API.Common.Schemas
{
    public static class Schema
    {
        public static class Projects
        {
            public const string TableName = "Projects";
            public const string ProjectTypeIdColumn = "ProjectTypeId";
            public const string ProjectIdColumn = "ProjectTypeId";
            public const string ProjectNameColumn = "Name";
        }
        
        public static class ProjectTypes
        {
            public const string TableName = "ProjectTypes";
            public const string ProjectTypeIdColumn = "ProjectTypeId";
            public const string ProjectTypeNameColumn = "Name";
        }
        
        public static class Fields
        {
            public const string TableName = "Fields";
            public const string LabelColumn = "Label";
            public const string PageIdColumn = "PageId";
        }
        
        public static class Pages
        {
            public const string TableName = "Pages";
            public const string PageNameColumn = "Label";
            public const string PageIdColumn = "PageId";
        }
    }
}