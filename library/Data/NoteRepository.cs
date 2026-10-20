using library.Models;
using Npgsql;

namespace library.Data;

// Every SQL statement about notes lives in this one class.
public class NoteRepository
{
    private readonly NpgsqlDataSource _dataSource;

    // ASP.NET Core hands in the data source that Program.cs registered.
    public NoteRepository(NpgsqlDataSource dataSource)
    {
        _dataSource = dataSource;
    }

    // Copies the current row of the reader into a Note.
    // The positions 0 to 3 match the column order in every SELECT in this class.
    private static Note ReadNote(NpgsqlDataReader reader)
    {
        return new Note
        {
            NoteId = reader.GetInt64(0),
            Title = reader.GetString(1),
            Body = reader.IsDBNull(2) ? null : reader.GetString(2), // NULL becomes null
            CreatedOn = DateOnly.FromDateTime(reader.GetDateTime(3))
        };
    }

    // READ: every note, newest first.
    public async Task<List<Note>> GetAllAsync()
    {
        const string sql = "SELECT note_id, title, body, created_on FROM lending.note " +
                           "ORDER BY created_on DESC, note_id DESC;";
        var notes = new List<Note>();
        // The command borrows a connection; 'await using' gives it back when the method ends.
        await using var command = _dataSource.CreateCommand(sql);
        await using var reader = await command.ExecuteReaderAsync();
        while (await reader.ReadAsync()) // once for each row
        {
            notes.Add(ReadNote(reader));
        }
        return notes;
    }
    // CREATE: insert a new note. The database chooses note_id and created_on.
    public async Task AddAsync(Note note)
    {
        const string sql = "INSERT INTO lending.note (title, body) VALUES (@title, @body);";
        await using var command = _dataSource.CreateCommand(sql);
        command.Parameters.AddWithValue("title", note.Title);
        // A C# null must be sent as DBNull.Value, which is how a database NULL is written.
        command.Parameters.AddWithValue("body", (object?)note.Body ?? DBNull.Value);
        await command.ExecuteNonQueryAsync(); // runs SQL that returns no rows
    }
}
