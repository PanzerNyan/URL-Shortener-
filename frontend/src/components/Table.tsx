import React, { useEffect, useState } from "react";
import axios from "axios";
import { Link } from "react-router-dom";

const Table: React.FC = () => {
  const [urls, setUrls] = useState<any[]>([]);
  const [newUrl, setNewUrl] = useState("");
  const [isLoggedIn, setIsLoggedIn] = useState(false);
  const [userId, setUserId] = useState("");
  const [userRole, setUserRole] = useState("");

  useEffect(() => {
    fetchUrls();
    const token = localStorage.getItem("token");
    setIsLoggedIn(!!token);

    if (token) {
      try {
        const decodedToken = JSON.parse(atob(token.split(".")[1]));
        setUserId(decodedToken.sub || "");
        setUserRole(decodedToken.role || "User");
      } catch (error) {
        console.error("Token parsing error:", error);
        setUserRole("User");
      }
    }
  }, []);

  const fetchUrls = async () => {
    try {
      const response = await axios.get("https://localhost:7167/api/urls");
      setUrls(response.data);
    } catch (error) {
      console.error("Data load error:", error);
    }
  };

  const handleCreateUrl = async () => {
    if (!newUrl.trim()) {
      alert("Please enter a valid URL.");
      return;
    }

    // Commented out authorization check
    // const token = localStorage.getItem("token");
    // if (!token) {
    //     alert("You must be logged in to create a short URL.");
    //     return;
    // }

    try {
      const response = await axios.post(
        "https://localhost:7167/api/urls",
        { originalUrl: newUrl }
        // { headers: { Authorization: `Bearer ${token}` } } // Commented out headers
      );

      if (response.status === 201) {
        alert(`Short URL Created: ${response.data.shortUrl}`);
        setNewUrl("");
        fetchUrls();
      } else {
        alert("Error creating short URL.");
      }
    } catch (error: any) {
      console.error("URL creation error:", error);
      alert(error.response?.data || "Failed to create short URL.");
    }
  };

  const handleDeleteUrl = async (id: number, ownerId: string) => {
    if (userRole !== "Admin" && ownerId !== userId) {
      alert("You can delete only your own URL");
      return;
    }

    try {
      const token = localStorage.getItem("token");
      await axios.delete(`https://localhost:7167/api/urls/${id}`, {
        headers: { Authorization: `Bearer ${token}` }
      });
      fetchUrls();
    } catch (error) {
      console.error("Deletion error:", error);
    }
  };

  return (
    <div>
      <h2>URL List</h2>

      {/* Short URL Creation */}
      {isLoggedIn && (
        <div style={styles.inputContainer}>
          <input
            type="text"
            placeholder="Enter URL..."
            value={newUrl}
            onChange={(e) => setNewUrl(e.target.value)}
            style={styles.input}
          />
          <button onClick={handleCreateUrl} style={styles.button}>
            Make Shorter
          </button>
        </div>
      )}

      {urls.length === 0 ? (
        <p>No URLs available.</p>
      ) : (
        <table style={styles.table}>
          <thead>
            <tr>
              <th style={styles.thtd}>ID</th>
              <th style={styles.thtd}>Original URL</th>
              <th style={styles.thtd}>Short URL</th>
              <th style={styles.thtd}>Details</th>
              <th style={styles.thtd}>Delete</th>
            </tr>
          </thead>
          <tbody>
            {urls.map((url) => (
              <tr key={url.id}>
                <td style={styles.thtd}>{url.id}</td>
                <td style={styles.thtd}>{url.fullUrl}</td>
                <td style={styles.thtd}>
                  <a
                    href={`https://localhost:7167/api/urls/${url.shortUrl}`}
                    target="_blank"
                    rel="noopener noreferrer"
                  >
                    {url.shortUrl}
                  </a>
                </td>
                <td style={styles.thtd}>
                  <Link to={`/url/${url.id}`} style={styles.button}>
                    Details
                  </Link>
                </td>
                <td style={styles.thtd}>
                  {(userRole === "Admin" || url.userID === userId) && (
                    <button
                      onClick={() => handleDeleteUrl(url.id, url.userID)}
                      style={styles.button}
                    >
                      Delete
                    </button>
                  )}
                </td>
              </tr>
            ))}
          </tbody>
        </table>
      )}
    </div>
  );
};

const styles = {
  table: {
    width: "100%",
    borderCollapse: "collapse" as const,
    marginTop: "20px"
  },
  thtd: {
    border: "1px solid black",
    padding: "8px",
    textAlign: "left" as const
  },
  button: {
    padding: "5px 10px",
    backgroundColor: "#007bff",
    color: "#fff",
    border: "none",
    cursor: "pointer",
    borderRadius: "5px",
    marginLeft: "5px"
  },
  inputContainer: {
    display: "flex",
    alignItems: "center",
    gap: "10px",
    marginBottom: "10px"
  },
  input: {
    padding: "8px",
    flex: 1,
    border: "1px solid #ccc",
    borderRadius: "5px"
  }
};

export default Table;