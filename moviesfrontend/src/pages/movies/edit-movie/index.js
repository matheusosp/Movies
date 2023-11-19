import api from "../../../core/interceptors/request-interceptor";
import {useEffect, useState} from "react";
import {useHistory, useParams} from "react-router-dom";

export default function Movie() {
    const { id } = useParams();
    const history = useHistory();
    const [filme, setFilme] = useState([]);
    const [loading, setLoading] = useState(true);

    useEffect(() => {
        async function loadMovie() {
            const response = await api.get(`r-api/?api=filmes/${id}`);

            if (response.data.length === 0) {
                history.replace("/");
                return;
            }

            setFilme(response.data);
            setLoading(false);
        }
        loadMovie();
        return () => {
            console.log("Componente Desmontado!");
        };
    }, [history, id]);

    if (loading) {
        return (
            <div className="filme-info">
                <h1>Carregando seu filme...</h1>
            </div>
        );
    }

    function salvarFilme(){
        const minhaLista = localStorage.getItem('filmes');

        let filmesSalvos = JSON.parse(minhaLista) || [];

        //Se tiver algum filme salvo com o mesmo ID, ignorar;

        const hasFilme = filmesSalvos.some((filmeSalvo) => filmeSalvo.id === filme.id);

        if(hasFilme){
            toast.info('Você já possui esse  filme salvo.');
            return;
            //Parar execução do código aqui
        }
        filmesSalvos.push(filme);
        localStorage.setItem('filmes', JSON.stringify(filmesSalvos));
        toast.success('Filme salvo com sucesso!');
    }
    return (
        <div className="filme-info">
            <h1>{filme.nome}</h1>
            <img src={filme.foto} alt={filme.nome} />
            <h3>Sinopse</h3>
            {filme.sinopse}
            <div className="botoes">
                <button onClick={salvarFilme}>Salvar</button>
                <button>
                    <a

                        href={`https://youtube.com/results?search_query=${filme.nome} Trailer`}
                    >
                        Trailer
                    </a>
                </button>
            </div>
        </div>
    );
}
