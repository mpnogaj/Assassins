import { NavComponent, NavComponentProps, navHOC } from '@/components/hoc/NavComponent';
import Endpoints from '@/endpoints';
import LoginDto from '@/types/dto/loginDto';
import { empty } from '@/types/other';
import { sendPost } from '@/utils/fetchUtils';
import React from 'react';

type State = {
	username: string;
	password: string;
};

class LoginPage extends NavComponent<empty, State> {
	constructor(props: NavComponentProps<empty>) {
		super(props);
		this.state = {
			username: '',
			password: ''
		};
	}

	loginHandler = async () => {
		const payload: LoginDto = {
			username: this.state.username,
			password: this.state.password
		};

		try {
			const result = await sendPost(Endpoints.user.login, payload);
			if (result.ok) this.props.navigate('/');
		} catch {
			/* empty */
		}
	};

	render(): React.ReactNode {
		return (
			<div className="container">
				<h1>Sign in ...</h1>
				<form
					className="form"
					onSubmit={event => {
						event.preventDefault();
						this.loginHandler();
					}}
				>
					<div className="form-group">
						<label>Login: </label>
						<input
							required
							className="form-control"
							type="text"
							value={this.state.username}
							onInput={e => {
								this.setState({ username: e.currentTarget.value });
							}}
						/>
					</div>
					<div className="form-group">
						<label>Password: </label>
						<input
							required
							className="form-control"
							type="password"
							value={this.state.password}
							onInput={e => {
								this.setState({ password: e.currentTarget.value });
							}}
						/>
					</div>
					<button type="submit" className="btn btn-primary mt-3">
						Login
					</button>
					<div>
						<span>
							Don&apos;t have an account? <a href="/register">Sign up</a>
						</span>
					</div>
				</form>
			</div>
		);
	}
}

export default navHOC(LoginPage);
