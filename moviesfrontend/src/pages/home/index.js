import { useEffect, useState } from "react";
import "./movies.css";
import {useParams, useHistory, Link, BrowserRouter} from "react-router-dom";
import api from "../../core/interceptors/request-interceptor";
import Modal from 'react-modal';

export default function Movies() {
    const [filmes, setFilmes] = useState([]);
    const [modalIsOpen, setModalIsOpen] = useState(false);
    const [selectedFilms, setSelectedFilms] = useState([]);
    const [formData, setFormData] = useState({
        name: "",
        active: true,
        genreId: 0,
    });
    const [selectedGenre, setSelectedGenre] = useState('');
    const [genres, setGenres] = useState([]);
    const data ={
        email: 'userMaster@gmail.com',
        password: '1234',
    };
    useEffect(() => {
        const authenticate = async () => {
            try {
                const response = await api.post(`/accounts/login`, data);

                if (response.status === 200) {
                    return response.data;
                } else {
                    throw new Error('Autenticação falhou');
                }
            } catch (error) {
                console.error('Erro ao autenticar:', error);
                throw error;
            }
        };

        const user = atob(localStorage.getItem('user'));
        if(!user || !user.accessToken || (new Date(user.expiration).getTime() < new Date().getTime())){
            authenticate()
                .then(data => {
                    localStorage.setItem('user', btoa(JSON.stringify(data)));
                })
                .catch(error => {
                    console.error('Erro durante a autenticação ou ao salvar no localStorage:', error);
                });

        }
    }, []);
    useEffect(() => {
        const fetchData = async () => {
            try {
                const moviesData = await api.get('/movies');
                setFilmes(moviesData.data);
            } catch (error) {
                console.error('Erro ao buscar filmes:', error);
            }
        };

        fetchData();
    }, []);

    useEffect(() => {
        const fetchGenres = async () => {
            try {
                const genresData = await api.get('/movies/genres');
                setGenres(genresData.data);
            } catch (error) {
                console.error('Erro ao buscar gêneros:', error);
            }
        };

        fetchGenres();
    }, []);
    const handleFilmClick = (filmId) => {
        setSelectedFilms((prevSelected) => {
            if (prevSelected.includes(filmId)) {
                return prevSelected.filter((id) => id !== filmId);
            } else {
                return [...prevSelected, filmId];
            }
        });
    };
    const handleDeleteSelected = async () => {
        try {
            await Promise.all(
                selectedFilms.map(async (filmId) => {
                    await api.delete(`/movies/${filmId}`);
                })
            );

            const moviesData = await api.get("/movies");
            setFilmes(moviesData.data);
            setSelectedFilms([]);
        } catch (error) {
            console.error("Erro ao excluir filmes:", error);
        }
    };
    const handleDeleteFilm = async (filmId) => {
        try {
            await api.delete(`/movies/${filmId}`);

            const moviesData = await api.get("/movies");
            setFilmes(moviesData.data);
        } catch (error) {
            console.error(`Erro ao excluir o filme ${filmId}:`, error);
        }
    };
    const handleInputChange = (e) => {
        const { name, value, type, checked } = e.target;
        const inputValue = type === 'checkbox' ? checked : value;

        setFormData((prevData) => ({
            ...prevData,
            [name]: inputValue,
        }));
    };
    const salvarNovoFilme = async () => {
        try {
            await api.post('/movies', {
                name: formData.name,
                active: formData.active,
                genreId: formData.genreId,
            });

            setModalIsOpen(false);
            const moviesData = await api.get('/movies');
            setFilmes(moviesData.data);
        } catch (error) {
            console.error('Erro ao adicionar novo filme:', error);
        }
    };
    return (
        <div className="container">
            <button onClick={() => setModalIsOpen(true)}>Adicionar Novo Filme</button>
            <div className="lista-filmes">
                {filmes.map((filme) => {
                    const dataRegistro = new Date(filme.registrationDate);

                    const dia = dataRegistro.getDate();
                    const mes = dataRegistro.getMonth() + 1;
                    const ano = dataRegistro.getFullYear();

                    const dataFormatada = `${dia < 10 ? "0" : ""}${dia}/${
                        mes < 10 ? "0" : ""
                    }${mes}/${ano}`;
                    return (
                        <div
                            key={filme.id}
                            className={`filme-card ${
                                selectedFilms.includes(filme.id) ? "selected" : ""
                            }`}
                            onClick={() => handleFilmClick(filme.id)}
                        >
                            <strong>Titulo: {filme.name}</strong>
                            <strong>Genero: {filme.genre.name}</strong>
                            <strong>Data de registro: {dataFormatada}</strong>
                            <div>
                                <Link to={{ pathname: `/movies/${filme.id}` }}>
                                    <button>Editar</button>
                                </Link>
                                <button onClick={() => handleDeleteFilm(filme.id)}>Excluir</button>
                            </div>

                        </div>
                    );
                })}
            </div>
            {selectedFilms.length > 0 && (
                <button onClick={handleDeleteSelected}>Excluir Filmes Selecionados</button>
            )}
            <Modal
                isOpen={modalIsOpen}
                onRequestClose={() => setModalIsOpen(false)}
                contentLabel="Adicionar Novo Filme"
            >
                {/* Conteúdo do modal */}
                <h2>Adicionar Novo Filme</h2>
                <label>
                    Título:
                    <input
                        type="text"
                        name="name"
                        value={formData.name}
                        onChange={handleInputChange}
                    />
                </label>
                <label>
                    Gênero:
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
                    Ativo:
                    <input
                        type="checkbox"
                        name="active"
                        checked={formData.active}
                        onChange={handleInputChange}
                    />
                </label>
                <button onClick={() => salvarNovoFilme()}>Salvar</button>
            </Modal>
        </div>
    );
}
