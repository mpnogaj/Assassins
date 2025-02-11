import { NavComponent, NavComponentProps, navHOC } from '@/components/hoc/NavComponent';
import Endpoints from '@/endpoints';
import RegisterDto from '@/types/dto/registerDto';
import { empty } from '@/types/other';
import { sendPost } from '@/utils/fetchUtils';
import { ACTION, RECAPTCHA_SCRIPT_ID, SITE_KEY } from '@/utils/recaptcha';
import React from 'react';
import toast from 'react-hot-toast';

type State = {
	username: string;
	password: string;
	passwordConfirmation: string;
	firstName: string;
	lastName: string;
};

class RegisterPage extends NavComponent<empty, State> {
	recaptchaScript: HTMLScriptElement | undefined;

	constructor(props: NavComponentProps<empty>) {
		super(props);

		this.state = {
			username: '',
			password: '',
			passwordConfirmation: '',
			firstName: '',
			lastName: ''
		};
	}

	componentDidMount(): void {
		this.recaptchaScript = document.querySelector(`#${RECAPTCHA_SCRIPT_ID}`) as HTMLScriptElement;
		if (!this.recaptchaScript) {
			const script = document.createElement('script');
			script.id = RECAPTCHA_SCRIPT_ID;
			script.src = `https://www.google.com/recaptcha/api.js?render=${SITE_KEY}`;
			script.defer = true;
			script.async = true;

			document.body.appendChild(script);

			script.addEventListener('load', function () {
				console.log('recaptcha loaded');
			});

			document.head.appendChild(script);
		}
	}

	componentWillUnmount(): void {
		if (this.recaptchaScript) {
			document.body.removeChild(this.recaptchaScript);
		}
	}

	registerHandler = async () => {
		if (!window.grecaptcha) {
			toast.error('reCAPTCHA not loaded yet. Please try again after few seconds');
			return;
		}

		try {
			const recaptchaToken = await window.grecaptcha.execute(SITE_KEY, { action: ACTION });

			console.log(recaptchaToken);

			const payload: RegisterDto = {
				recaptchaToken: recaptchaToken,
				username: this.state.username,
				password: this.state.password,
				firstName: this.state.firstName,
				lastName: this.state.lastName
			};

			const result = await sendPost(Endpoints.user.register, payload);
			if (result.ok) this.props.navigate('/');
		} catch {
			toast.error('Something went wrong. Please try again');
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
					<div className="form-group">
						<label>Confirm password: </label>
						<input
							required
							className="form-control"
							type="password"
							value={this.state.passwordConfirmation}
							onChange={event => {
								this.setState({ passwordConfirmation: event.target.value });
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
