import { NavComponent, NavComponentProps, navHOC } from '@/components/hoc/NavComponent';
import Endpoints from '@/endpoints';
import RegisterDto from '@/types/dto/registerDto';
import { empty } from '@/types/other';
import { sendPost } from '@/utils/fetchUtils';
import React from 'react';

type State = {
	username: string;
	password: string;
	firstName: string;
	lastName: string;
};

class RegisterPage extends NavComponent<empty, State> {
	constructor(props: NavComponentProps<empty>) {
		super(props);

		this.state = {
			username: '',
			password: '',
			firstName: '',
			lastName: ''
		};
	}

	registerHandler = async () => {
		const payload: RegisterDto = {
			username: this.state.username,
			password: this.state.password,
			firstName: this.state.firstName,
			lastName: this.state.lastName
		};

		try {
			const result = await sendPost(Endpoints.user.register, payload);
			if (result.ok) this.props.navigate('/');
		} catch {
			/* empty */
		}
	};

	render(): React.ReactNode {
		return (
			<div className="container">
				<h1>Register</h1>
				<form
					className="form"
					onSubmit={event => {
						event.preventDefault();
						this.registerHandler();
					}}
				>
					<div className="form-group">
						<label>First name: </label>
						<input
							required
							className="form-control"
							type="text"
							value={this.state.firstName}
							onChange={event => {
								this.setState({ firstName: event.target.value });
							}}
						/>
					</div>
					<div className="form-group">
						<label>Last name: </label>
						<input
							required
							className="form-control"
							type="text"
							value={this.state.lastName}
							onChange={event => {
								this.setState({ lastName: event.target.value });
							}}
						/>
					</div>
					<div className="form-group">
						<label>Username: </label>
						<input
							required
							className="form-control"
							type="text"
							value={this.state.username}
							onChange={event => {
								this.setState({ username: event.target.value });
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
							onChange={event => {
								this.setState({ password: event.target.value });
							}}
						/>
					</div>
					<button type="submit" className="btn btn-primary mt-3">
						Register
					</button>
					<div>
						<span>
							Already have an account? <a href="/login">Sign in</a>
						</span>
					</div>
				</form>
			</div>
		);
	}
}

export default navHOC(RegisterPage);
