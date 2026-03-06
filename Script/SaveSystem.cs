using Godot;
using System.Collections.Generic;
using System.Text.Json;

/// <summary>
/// Système de sauvegarde de la position et de l'inventaire du joueur.
/// Attache ce script à un AutoLoad (singleton) dans ton projet.
/// </summary>
public partial class SaveSystem : Node
{
    private const string SAVE_PATH = "user://save.json";

    // Classe qui contient toutes les données à sauvegarder
    private class SaveData
    {
        public float X { get; set; }
        public float Y { get; set; }
        public float Z { get; set; }
        public List<string> Items { get; set; } = new List<string>();
    }

    // ─── Sauvegarder position + inventaire ────────────────────────────────────
    public void Save(Vector3 position, List<string> items)
    {
        var data = new SaveData
        {
            X = position.X,
            Y = position.Y,
            Z = position.Z,
            Items = items
        };
        string json = JsonSerializer.Serialize(data);
        using var file = FileAccess.Open(SAVE_PATH, FileAccess.ModeFlags.Write);
        file.StoreString(json);
        GD.Print($"[Save] Position : {position} | Items : {items.Count}");
    }

    // ─── Charger la position ───────────────────────────────────────────────────
    public Vector3? LoadPosition()
    {
        if (!FileAccess.FileExists(SAVE_PATH))
        {
            GD.Print("[Save] Aucune sauvegarde trouvée.");
            return null;
        }

        using var file = FileAccess.Open(SAVE_PATH, FileAccess.ModeFlags.Read);
        var data = JsonSerializer.Deserialize<SaveData>(file.GetAsText());
        var position = new Vector3(data.X, data.Y, data.Z);
        GD.Print($"[Save] Position chargée : {position}");
        return position;
    }

    // ─── Charger l'inventaire ──────────────────────────────────────────────────
    public List<string> LoadInventory()
    {
        if (!FileAccess.FileExists(SAVE_PATH))
        {
            GD.Print("[Save] Aucune sauvegarde trouvée.");
            return new List<string>();
        }

        using var file = FileAccess.Open(SAVE_PATH, FileAccess.ModeFlags.Read);
        var data = JsonSerializer.Deserialize<SaveData>(file.GetAsText());
        GD.Print($"[Save] Inventaire chargé : {data.Items.Count} item(s)");
        return data.Items;
    }

    // ─── Reset la sauvegarde ───────────────────────────────────────────────────
    public void ResetSave()
    {
        if (FileAccess.FileExists(SAVE_PATH))
        {
            DirAccess.RemoveAbsolute(ProjectSettings.GlobalizePath(SAVE_PATH));
            GD.Print("[Save] Sauvegarde supprimée.");
        }
        else
        {
            GD.Print("[Save] Aucune sauvegarde à supprimer.");
        }
    }

    public bool HasSave() => FileAccess.FileExists(SAVE_PATH);
}