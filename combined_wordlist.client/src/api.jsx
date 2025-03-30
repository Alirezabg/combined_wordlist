import axios from "axios";


export const makeGuess = async (guess) => {
    try {
        const response = await fetch('api/wordle/guess', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
                'Accept': '*/*',
            },
            body: JSON.stringify(guess),
        });

        if (!response.ok) {
            throw new Error(`Server responded with ${response.status}`);
        }

        const data = await response.json();
        return data;
    } catch (error) {
        console.error("Error making guess:", error);
        return { response: "Error connecting to server.", gameOver: false };
    }
};


export const resetGame = async () => {
    try {
        await axios.get(`api/wordle/reset`);
    } catch (error) {
        console.error("Error resetting game:", error);
    }
};
export const getHelp = async () => {
    try {
        const response = await axios.get(`api/wordle/help`);
        const data = await response.data;
        return data;
    } catch (error) {
        console.error("Error getting help:", error);
        return { response: "Error connecting to server." };
    }
}
