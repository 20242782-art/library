using System.ComponentModel.DataAnnotations;

namespace library.Models;

// One Note object holds one row of lending.note.
public class Note
{
    public long NoteId { get; set; }            // note_id BIGINT, the primary key

    [StringLength(100)]
    public string Title { get; set; } = "";     // title VARCHAR(100) NOT NULL

    public string? Body { get; set; }           // body TEXT, may be NULL

    public DateOnly CreatedOn { get; set; }     // created_on DATE, filled by the database
}