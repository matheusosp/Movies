import {BrowserRouter, Route, Switch} from 'react-router-dom';
import Movies from "./pages/Movies";
import Header from "./shared/components/Header/header";

const Routes = () =>{
    return(
        <BrowserRouter>
            <Header/>
            <Switch>
                <Route exact path='/' component={Movies}/>
            </Switch>
        </BrowserRouter>
    )
}

export default Routes;