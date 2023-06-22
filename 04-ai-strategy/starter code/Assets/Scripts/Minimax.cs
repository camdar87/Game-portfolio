// Minimax.cs
//
// This script implements the Minimax algorithm for a chess game AI. It evaluates and generates possible moves.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minimax : MonoBehaviour
{
    BoardManager board;
    GameManager gameManager;
    MoveData bestMove;
    int myScore = 0;
    int opponentScore = 0;
    int maxDepth;


     int count; //count

    List<TileData> myPieces = new List<TileData>();
    List<TileData> opponentPieces = new List<TileData>();
    Stack<MoveData> moveStack = new Stack<MoveData>();
    MoveHeuristic weight = new MoveHeuristic();

    public static Minimax instance;
    public static Minimax Instance
    {
        get { return instance; }
    }

    // Awake is called when the script instance is being loaded
    private void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(this);
    }

    // Create a move object from a source tile and a target tile
    MoveData CreateMove(TileData from, TileData to)
    {
        MoveData tempMove = new MoveData
        {
            firstPosition = from,
            pieceMoved = from.CurrentPiece,
            secondPosition = to
        };

        if (to.CurrentPiece != null)
            tempMove.pieceKilled = to.CurrentPiece;

            
        Debug.Log(count);
        return tempMove;
    }

    // Get all possible moves for a given player's team.
    List<MoveData> GetMoves(PlayerTeam team)
    {
        List<MoveData> turnMove = new List<MoveData>();
        List<TileData> pieces = (team == gameManager.playerTurn) ? myPieces : opponentPieces;

        foreach (TileData tile in pieces)
        {
            MoveFunction movement = new MoveFunction(board);
            List<MoveData> pieceMoves = movement.GetMoves(tile.CurrentPiece, tile.Position);

            foreach (MoveData move in pieceMoves)
            {
                MoveData newMove = CreateMove(move.firstPosition, move.secondPosition);
                turnMove.Add(newMove);
            }
        }
        return turnMove;
    }


    void DoFakeMove(TileData currentTile, TileData targetTile)
    {
        targetTile.SwapFakePieces(currentTile.CurrentPiece);
        currentTile.CurrentPiece = null;
    }

 
    void UndoFakeMove()
    {
        MoveData tempMove = moveStack.Pop();
        TileData movedTo = tempMove.secondPosition;
        TileData movedFrom = tempMove.firstPosition;
        ChessPiece pieceKilled = tempMove.pieceKilled;
        ChessPiece pieceMoved = tempMove.pieceMoved;

        movedFrom.CurrentPiece = movedTo.CurrentPiece;
        movedTo.CurrentPiece = (pieceKilled != null) ? pieceKilled : null;
    }

    // Evaluate the current board state by calculating the difference in piece scores between the players.
    int Evaluate()
    {
        int pieceDifference = myScore - opponentScore;
        return pieceDifference;
    }

    // Update the lists of player pieces and their scores based on the current board state.
    void GetBoardState()
    {
        myPieces.Clear();
        opponentPieces.Clear();
        myScore = 0;
        opponentScore = 0;

        for (int y = 0; y < 8; y++)
        {
            for (int x = 0; x < 8; x++)
            {
                TileData tile = board.GetTileFromBoard(new Vector2(x, y));
                if (tile.CurrentPiece != null && tile.CurrentPiece.Type != ChessPiece.PieceType.NONE)
                {
                    if (tile.CurrentPiece.Team == gameManager.playerTurn)
                    {
                        myScore += weight.GetPieceWeight(tile.CurrentPiece.Type);
                        myPieces.Add(tile);
                    }
                    else
                    {
                        opponentScore += weight.GetPieceWeight(tile.CurrentPiece.Type);
                        opponentPieces.Add(tile);
                    }
                }
            }
        }
    }

    // Get the best move for the AI player using the Minimax algorithm.
    public MoveData GetMove()
    {
        board = BoardManager.Instance;
        gameManager = GameManager.Instance;
        bestMove = CreateMove(board.GetTileFromBoard(new Vector2(0, 0)), board.GetTileFromBoard(new Vector2(0, 0)));
        count = 0; //debug count
        maxDepth = 3;
        CalculateMinMax(maxDepth, int.MinValue, int.MaxValue, true);

        return bestMove;
    }

    // Calculate the Minimax value for the current game state recursively using alpha-beta pruning.
    int CalculateMinMax(int depth, int alpha, int beta, bool max)
    {
        count++; //Prune debug count
        GetBoardState();

        if (depth == 0)
            return Evaluate();

        if (max)
        {
            List<MoveData> allMoves = GetMoves(gameManager.playerTurn);
            allMoves = Shuffle(allMoves);
            foreach (MoveData move in allMoves)
            {
                moveStack.Push(move);

                DoFakeMove(move.firstPosition, move.secondPosition);
                int score = CalculateMinMax(depth - 1, alpha, beta, false);
                UndoFakeMove();
                if (score > alpha)
                {
                    alpha = score;
                    move.score = score;
                }

                if (score > bestMove.score && depth == maxDepth)
                {
                    bestMove = move;
                }

                if (score >= beta)
                    break;
            }
            return alpha;
        }
        else
        {
            PlayerTeam opponent = gameManager.playerTurn == PlayerTeam.WHITE ? PlayerTeam.BLACK : PlayerTeam.WHITE;
            List<MoveData> allMoves = GetMoves(opponent);
            allMoves = Shuffle(allMoves);
            foreach (MoveData move in allMoves)
            {
                moveStack.Push(move);

                DoFakeMove(move.firstPosition, move.secondPosition);
                int score = CalculateMinMax(depth - 1, alpha, beta, true);
                UndoFakeMove();

                if (score < beta)
                    beta = score;

                if (score <= alpha)
                    break;
            }
            return beta;
        }
    }

    // Shuffle the elements in a list using the Fisher-Yates algorithm.
    public List<T> Shuffle<T>(List<T> list)
    {
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = Random.Range(0, n);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
        return list;
    }

}
