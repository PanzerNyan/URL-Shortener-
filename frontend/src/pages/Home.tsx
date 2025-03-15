import React from "react";
import Header from "../components/Header";
import Table from "../components/Table";

const Home: React.FC = () => {
    return (
        <div>
            <Header />
            <main style={styles.main}>
                <Table />
            </main>cd frontend
        </div>
    );
};

const styles = {
    main: {
        padding: "20px",
    },
};

export default Home;