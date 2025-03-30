import React, { useState } from "react";
import { makeGuess, resetGame, getHelp, solveGame } from "./api";

const WordleGame = () => {
    const [guess, setGuess] = useState("");
    const [history, setHistory] = useState([]);
    const [hint, setHint] = useState([]);
    const [gameOver, setGameOver] = useState(false);
    const [computerSteps, setComputerSteps] = useState([]);


    const handleHelp = async () => {
        const result = await getHelp();
        if (result.message) {
            console.log("no more valid")
        }else if (result) {
            // Format the hint nicely
            const formattedHint = `Letter at position ${result.position + 1}: ${result.letter.toUpperCase()}`;
            setHint(prev => [...prev, formattedHint]);
        }
    }
    const handleSolve = async () => {
        const result = await solveGame();
        setComputerSteps(result.steps || []);
    };
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
        setHint([])
        setComputerSteps([])
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
            <button onClick={handleHelp}>Get Help</button>
            {hint.map((entry, index) =>
                (<p key={index}>{entry} </p>))}
            <button onClick={handleSolve}>Let's Computer Solve It</button>

            {computerSteps.length > 0 && (
                <div className="computer-solver">
                    <h3>Computer Solver Result:</h3>
                    {computerSteps.map((step, index) => (
                        <p key={index}>
                            <strong>{step.guess.toUpperCase()}:</strong> {step.hint}
                        </p>
                    ))}
                </div>
            )}
            <button onClick={handleReset} className="reset-btn">Reset Game</button>
        </div>
    );
};

export default WordleGame;
