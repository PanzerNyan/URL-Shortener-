import React, { useState } from "react";
import axios from "axios";
import { CSSProperties } from "react";

const Register: React.FC = () => {
  const [username, setUsername] = useState("");
  const [password, setPassword] = useState("");
  const [error, setError] = useState("");

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setError("");

    try {
      await axios.post("https://localhost:7167/api/auth/register", {
        username,
        password,
      });
      alert("Registration Successful!");
      window.location.href = "/login"; // Transfer to Login
    } catch (err) {
      setError("Registration Error. User Already Exists.");
    }
  };

  return (
    <div style={styles.container}>
      <h2>Account Registration</h2>
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
        <button type="submit" style={styles.button}>Register</button>
      </form>
    </div>
  );
};

const styles: { container: CSSProperties; input: CSSProperties; button: CSSProperties; error: CSSProperties } = {
    container: {
      maxWidth: "300px",
      margin: "auto",
      padding: "20px",
      textAlign: "center" as const, 
    },
    input: {
      width: "100%",
      padding: "10px",
      marginBottom: "10px",
    },
    button: {
      width: "100%",
      padding: "10px",
      background: "#4CAF50",
      color: "white",
      cursor: "pointer",
    },
    error: {
      color: "red",
    },
  };
  

export default Register;