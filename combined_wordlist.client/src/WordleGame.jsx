import React, { useState } from "react";
import { makeGuess, resetGame } from "./api";

const WordleGame = () => {
    const [guess, setGuess] = useState("");
    const [history, setHistory] = useState([]);
    const [gameOver, setGameOver] = useState(false);

    const handleGuess = async () => {
        if (gameOver || !guess.trim()) return;

        const result = await makeGuess(guess.toLowerCase());
        setHistory([...history, { guess, feedback: result.response }]);

        if (result.gameOver) {
            setGameOver(true);
        }

        setGuess("");
    };

    const handleReset = async () => {
        await resetGame();
        setHistory([]);
        setGameOver(false);
        setGuess("");
    };

    return (
        <div className="wordle-container">
            <h1>Wordle Clone</h1>
            <div className="input-container">
                <input
                    type="text"
                    value={guess}
                    maxLength={5}
                    onChange={(e) => setGuess(e.target.value)}
                    disabled={gameOver}
                    placeholder="Enter a 5-letter word"
                />
                <button onClick={handleGuess} disabled={gameOver}>Guess</button>
            </div>

            <div className="history">
                {history.map((entry, index) => (
                    <p key={index}>
                        <strong>{entry.guess.toUpperCase()}:</strong> {entry.feedback}
                    </p>
                ))}
            </div>

            {gameOver && <h2>Game Over! ??</h2>}

            <button onClick={handleReset} className="reset-btn">Reset Game</button>
        </div>
    );
};

export default WordleGame;
