namespace AwningsAPI.Dto.Workflow
{
    /// <summary>
    /// Returned by DELETE /api/workflow/DeleteWorkflow/{id}.
    ///
    /// When <see cref="Deleted"/> is true the workflow was removed.
    /// When false it was blocked; <see cref="BlockingDependencies"/> lists every
    /// dependency type that prevents deletion so the UI can show a clear message.
    /// </summary>
    public class WorkflowDeleteResult
    {
        /// <summary>True = workflow was deleted successfully.</summary>
        public bool Deleted { get; set; }

        /// <summary>Human-readable summary, e.g. "Workflow deleted successfully."</summary>
        public string Message { get; set; } = string.Empty;

        /// <summary>
        /// Names of dependency types that are blocking deletion.
        /// Empty when <see cref="Deleted"/> is true.
        /// Possible values: "Initial Enquiry", "Quote", "Showroom Visit",
        ///                   "Site Visit", "Invoice".
        /// </summary>
        public List<WorkflowDependency> BlockingDependencies { get; set; } = new();
    }

    /// <summary>One dependency type that is blocking deletion.</summary>
    public class WorkflowDependency
    {
        /// <summary>Display name shown in the UI, e.g. "Quote".</summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>How many records of this type exist for the workflow.</summary>
        public int Count { get; set; }
    }
}