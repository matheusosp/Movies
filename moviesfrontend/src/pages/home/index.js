import { useEffect, useState } from "react";
import "./movies.css";
import {useParams, useHistory, Link, BrowserRouter} from "react-router-dom";
import api from "../../core/interceptors/request-interceptor";

export default function Movies() {
    const [filmes, setFilmes] = useState([]);
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

        const user = JSON.parse(atob(localStorage.getItem('user')));
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


    return (
        <div className="container">
            <div className="lista-filmes">
                {filmes.map((filme) => {
                    const dataRegistro = new Date(filme.registrationDate);

                    const dia = dataRegistro.getDate();
                    const mes = dataRegistro.getMonth() + 1;
                    const ano = dataRegistro.getFullYear();

                    const dataFormatada = `${dia < 10 ? '0' : ''}${dia}/${mes < 10 ? '0' : ''}${mes}/${ano}`;
                    return (
                        <article key={filme.id}>
                            <strong>Titulo: {filme.name}</strong>
                            <strong>Genero: {filme.genre.name}</strong>
                            <strong>Data de registro: {dataFormatada}</strong>>
                            <Link to={{pathname: `/movies/${filme.id}`}}>Editar</Link>
                        </article>
                    );
                })}
            </div>
        </div>
    );
}
