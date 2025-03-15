import React, { useState } from "react";
import axios from "axios";

const Login: React.FC = () => {
  const [username, setUsername] = useState("");
  const [password, setPassword] = useState("");
  const [error, setError] = useState("");

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setError("");

    try {
      const response = await axios.post("https://localhost:7167/api/auth/login", {
        username,
        password,
      });

      localStorage.setItem("token", response.data.token); // Save token
      alert("Successful login!");
      window.location.href = "/"; // Redirect to home
    } catch (err) {
      setError("Incorrect login or password");
    }
  };

  return (
    <div style={styles.container}>
      <h2>LogIn</h2>
      {error && <p style={styles.error}>{error}</p>}
      <form onSubmit={handleSubmit}>
        <input
          type="text"
          placeholder="Login"
          value={username}
          onChange={(e) => setUsername(e.target.value)}
          required
          style={styles.input}
        />
        <input
          type="password"
          placeholder="Password"
          value={password}
          onChange={(e) => setPassword(e.target.value)}
          required
          style={styles.input}
        />
        <button type="submit" style={styles.button}>LogIn</button>
      </form>
    </div>
  );
};

const styles = {
  container: 
{ 
    maxWidth: "300px", 
    margin: "auto", 
    padding: "20px", 
    textAlign: "center" as const 
},
  input: 
{ 
    width: "100%", 
    padding: "10px", 
    marginBottom: "10px" 
},
  button: 
{ 
    width: "100%", 
    padding: "10px", 
    background: "#4CAF50", 
    color: "white", 
    cursor: "pointer" 
},
  error: 
{ 
    color: "red" 
},
};

export default Login;

export {};