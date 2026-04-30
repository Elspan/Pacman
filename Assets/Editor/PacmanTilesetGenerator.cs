using UnityEngine;
using UnityEditor;

public class PacmanTilesetGenerator : MonoBehaviour
{
    // Couleurs Pacman
    static Color black = new Color(0, 0, 0, 1);
    static Color wallBlue = new Color(0.13f, 0.13f, 1f, 1);      // #2121FF
    static Color wallEdge = new Color(0.38f, 0.38f, 1f, 1);      // #6161FF

    static int size = 16; // taille de chaque tile en pixels

    [MenuItem("Pacman/Generate Tileset")]
    public static void GenerateTileset()
    {
        GenerateTile("wall_horizontal", DrawHorizontal);
        GenerateTile("wall_vertical", DrawVertical);
        GenerateTile("wall_corner_TL", DrawCornerTL);
        GenerateTile("wall_corner_TR", DrawCornerTR);
        GenerateTile("wall_corner_BL", DrawCornerBL);
        GenerateTile("wall_corner_BR", DrawCornerBR);
        GenerateTile("wall_full", DrawFull);
        GenerateTile("floor", DrawFloor);

        AssetDatabase.Refresh();
        Debug.Log("Tileset généré dans Assets/PacmanTiles/");
    }

    static void GenerateTile(string name, System.Action<Texture2D> drawFunc)
    {
        Texture2D tex = new Texture2D(size, size);
        tex.filterMode = FilterMode.Point; // pixel art net

        // Remplis de noir par défaut
        for (int x = 0; x < size; x++)
            for (int y = 0; y < size; y++)
                tex.SetPixel(x, y, black);

        drawFunc(tex);
        tex.Apply();

        // Sauvegarde en PNG
        byte[] bytes = tex.EncodeToPNG();
        string path = $"Assets/PacmanTiles/{name}.png";
        System.IO.Directory.CreateDirectory("Assets/PacmanTiles");
        System.IO.File.WriteAllBytes(path, bytes);
    }

    // Mur horizontal ══
    static void DrawHorizontal(Texture2D t)
    {
        for (int x = 0; x < size; x++)
        {
            t.SetPixel(x, 10, wallEdge);  // bord haut
            t.SetPixel(x, 9,  wallBlue);
            t.SetPixel(x, 8,  wallBlue);
            t.SetPixel(x, 7,  wallBlue);
            t.SetPixel(x, 6,  wallEdge);  // bord bas
        }
    }

    // Mur vertical ║
    static void DrawVertical(Texture2D t)
    {
        for (int y = 0; y < size; y++)
        {
            t.SetPixel(5,  y, wallEdge);
            t.SetPixel(6,  y, wallBlue);
            t.SetPixel(7,  y, wallBlue);
            t.SetPixel(8,  y, wallBlue);
            t.SetPixel(9,  y, wallEdge);
        }
    }

    // Coin haut-gauche ╔
    static void DrawCornerTL(Texture2D t)
    {
        // Bras droit (horizontal)
        for (int x = 8; x < size; x++)
        {
            t.SetPixel(x, 10, wallEdge);
            t.SetPixel(x, 9,  wallBlue);
            t.SetPixel(x, 8,  wallBlue);
            t.SetPixel(x, 7,  wallBlue);
            t.SetPixel(x, 6,  wallEdge);
        }
        // Bras bas (vertical)
        for (int y = 0; y <= 8; y++)
        {
            t.SetPixel(5, y, wallEdge);
            t.SetPixel(6, y, wallBlue);
            t.SetPixel(7, y, wallBlue);
            t.SetPixel(8, y, wallBlue);
            t.SetPixel(9, y, wallEdge);
        }
        // Arrondi du coin
        t.SetPixel(9, 9, wallBlue);
        t.SetPixel(9, 10, wallEdge);
        t.SetPixel(10, 9, wallEdge);
    }

    // Coin haut-droit ╗
    static void DrawCornerTR(Texture2D t)
    {
        for (int x = 0; x <= 8; x++)
        {
            t.SetPixel(x, 10, wallEdge);
            t.SetPixel(x, 9,  wallBlue);
            t.SetPixel(x, 8,  wallBlue);
            t.SetPixel(x, 7,  wallBlue);
            t.SetPixel(x, 6,  wallEdge);
        }
        for (int y = 0; y <= 8; y++)
        {
            t.SetPixel(6,  y, wallEdge);
            t.SetPixel(7,  y, wallBlue);
            t.SetPixel(8,  y, wallBlue);
            t.SetPixel(9,  y, wallBlue);
            t.SetPixel(10, y, wallEdge);
        }
        t.SetPixel(6, 9, wallBlue);
        t.SetPixel(6, 10, wallEdge);
        t.SetPixel(5, 9, wallEdge);
    }

    // Coin bas-gauche ╚
    static void DrawCornerBL(Texture2D t)
    {
        for (int x = 8; x < size; x++)
        {
            t.SetPixel(x, 9,  wallEdge);
            t.SetPixel(x, 8,  wallBlue);
            t.SetPixel(x, 7,  wallBlue);
            t.SetPixel(x, 6,  wallBlue);
            t.SetPixel(x, 5,  wallEdge);
        }
        for (int y = 8; y < size; y++)
        {
            t.SetPixel(5, y, wallEdge);
            t.SetPixel(6, y, wallBlue);
            t.SetPixel(7, y, wallBlue);
            t.SetPixel(8, y, wallBlue);
            t.SetPixel(9, y, wallEdge);
        }
        t.SetPixel(9, 8, wallBlue);
        t.SetPixel(9, 9, wallEdge);
        t.SetPixel(10, 8, wallEdge);
    }

    // Coin bas-droit ╝
    static void DrawCornerBR(Texture2D t)
    {
        for (int x = 0; x <= 8; x++)
        {
            t.SetPixel(x, 9,  wallEdge);
            t.SetPixel(x, 8,  wallBlue);
            t.SetPixel(x, 7,  wallBlue);
            t.SetPixel(x, 6,  wallBlue);
            t.SetPixel(x, 5,  wallEdge);
        }
        for (int y = 8; y < size; y++)
        {
            t.SetPixel(6,  y, wallEdge);
            t.SetPixel(7,  y, wallBlue);
            t.SetPixel(8,  y, wallBlue);
            t.SetPixel(9,  y, wallBlue);
            t.SetPixel(10, y, wallEdge);
        }
        t.SetPixel(6, 8, wallBlue);
        t.SetPixel(6, 9, wallEdge);
        t.SetPixel(5, 8, wallEdge);
    }

    // Mur plein (bloc)
    static void DrawFull(Texture2D t)
    {
        for (int x = 0; x < size; x++)
            for (int y = 0; y < size; y++)
                t.SetPixel(x, y, wallBlue);
        // Bordures
        for (int i = 0; i < size; i++)
        {
            t.SetPixel(i, 0, wallEdge);
            t.SetPixel(i, size-1, wallEdge);
            t.SetPixel(0, i, wallEdge);
            t.SetPixel(size-1, i, wallEdge);
        }
    }

    // Sol (noir)
    static void DrawFloor(Texture2D t) { } // déjà noir par défaut
}