import { useEffect, useState } from "react";
import "./movies.css";
import {useParams, useHistory, Link} from "react-router-dom";
import {fetchMovies} from "./movie.service";

export default function Movies() {
    const [filmes, setFilmes] = useState([]);

    useEffect(() => {
        const fetchData = async () => {
            try {
                const moviesData = await fetchMovies();
                setFilmes(moviesData);
            } catch (error) {
                // Handle error
                console.error('Erro ao buscar filmes:', error);
            }
        };

        fetchData();
    }, []);

    return (
        <div className="container">
            <div className="lista-filmes">
                {filmes.map((filme) => {
                    return (
                        <article key={filme.id}>
                            <strong>{filme.name}</strong>
                            <strong>{filme.genre}</strong>
                            <Link to={`/filme/${filme.id}`}>Acessar</Link>
                        </article>
                    );
                })}
            </div>
        </div>
    );
}
