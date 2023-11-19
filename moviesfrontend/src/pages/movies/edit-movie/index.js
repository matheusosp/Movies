import api from "../../../core/interceptors/request-interceptor";
import { useEffect, useState } from "react";
import { Link, useParams, useHistory } from "react-router-dom";
import "./edit-movie.css";

export default function Movie() {
    const { id } = useParams();
    const history = useHistory();
    const [filme, setFilme] = useState({});
    const [loading, setLoading] = useState(true);
    const [formData, setFormData] = useState({
        name: "",
        active: true,
        genreId: 0,
    });
    const [genres, setGenres] = useState([]);
    const [selectedGenre, setSelectedGenre] = useState('');

    useEffect(() => {
        const fetchGenres = async () => {
            try {
                const genresData = await api.get('/movies/genres');
                setGenres(genresData.data);
            } catch (error) {
                console.error('Erro ao buscar gêneros:', error);
            }
        };
        const fetchMovie = async () => {
            try {
                const response = await api.get(`/movies/${id}`);
                setFilme(response.data);
                setFormData({
                    name: response.data.name,
                    genre: response.data.genre.name,
                    genreId: response.data.genre.id,
                    active: response.data.active,
                });
                setSelectedGenre(response.data.genre.name);
                setLoading(false);
            } catch (error) {
                console.error('Erro ao buscar filme:', error);
            }
        };
        fetchGenres();
        fetchMovie();
    }, []);

    const handleInputChange = (e) => {
        const { name, value, type, checked } = e.target;
        setFormData((prevData) => ({
            ...prevData,
            [name]: value,
        }));
    };

    const salvarFilme = async () => {
        await api.put(`/movies/${id}`, {
            name: formData.name,
            active: formData.active,
            genreId: formData.genreId,
        });

        history.push("/");
    };

    if (loading) {
        return (
            <div className="filme-info">
                <h1>Carregando seu filme...</h1>
            </div>
        );
    }

    return (
        <article key={filme.id}>
            <form>
                <div className="filme-info">
                    <label>
                        <h1>Título:</h1>
                        <input
                            type="text"
                            name="name"
                            value={formData.name}
                            onChange={handleInputChange}
                        />
                    </label>
                    <label>
                        <h1>Gênero:</h1>
                        <select
                            value={selectedGenre}
                            onChange={(e) => {
                                setSelectedGenre(e.target.value);
                                setFormData((prevData) => ({
                                    ...prevData,
                                    genreId: genres.find((genre) => genre.name === e.target.value)?.id || "",
                                }));
                            }}
                        >
                            <option value="" disabled>
                                Selecione um gênero
                            </option>
                            {genres.map((genre) => (
                                <option key={genre.id} value={genre.name}>
                                    {genre.name}
                                </option>
                            ))}
                        </select>
                    </label>
                    <label>
                        <h1>Ativo:</h1>
                        <input
                            type="checkbox"
                            name="active"
                            checked={formData.active}
                            onChange={() => {
                                setFormData((prevData) => ({
                                    ...prevData,
                                    active: !prevData.active,
                                }));
                            }}
                        />
                    </label>
                    <button type="button" onClick={salvarFilme}>
                        Salvar
                    </button>
                    <Link to="/">
                        <button type="button">Voltar</button>
                    </Link>
                </div>
            </form>
        </article>
    );
}
