import { NavComponent, NavComponentProps, navHOC } from '@/components/hoc/NavComponent';
import Endpoints from '@/endpoints';
import RegisterDto from '@/types/dto/registerDto';
import { empty } from '@/types/other';
import { sendPost } from '@/utils/fetchUtils';
import { ACTION, RECAPTCHA_SCRIPT_ID, SITE_KEY } from '@/utils/recaptcha';
import React from 'react';

type State = {
	username: string;
	password: string;
	confirmedPassword: string;
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
			confirmedPassword: '',
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
			console.error('reCAPTCHA not loaded yet.');
			return;
		} else if (this.state.password !== this.state.confirmedPassword) {
			alert('Passwords do not match.');
			console.error('Passwords do not match.');
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
			/* empty */
		}
	};

	render(): React.ReactNode {
		return (
			<div className="flex min-w-screen items-center justify-center">
				<div className="w-full max-w-2xl space-y-6 rounded-lg bg-gray-800 p-8 shadow-lg">
					<h1 className="text-center text-2xl font-semibold text-gray-400">Register</h1>
					<form
						className="space-y-4"
						onSubmit={event => {
							event.preventDefault();
							this.registerHandler();
						}}
					>
						<div>
							<label className="block text-sm font-medium text-gray-500">First name: </label>
							<input
								required
								className="mt-1 w-full rounded-md bg-gray-600 p-2"
								type="text"
								value={this.state.firstName}
								onChange={event => {
									this.setState({ firstName: event.target.value });
								}}
							/>
						</div>
						<div>
							<label className="block text-sm font-medium text-gray-500">Last name: </label>
							<input
								required
								className="mt-1 w-full rounded-md bg-gray-600 p-2"
								type="text"
								value={this.state.lastName}
								onChange={event => {
									this.setState({ lastName: event.target.value });
								}}
							/>
						</div>
						<div>
							<label className="block text-sm font-medium text-gray-500">Username: </label>
							<input
								required
								className="mt-1 w-full rounded-md bg-gray-600 p-2"
								type="text"
								value={this.state.username}
								onChange={event => {
									this.setState({ username: event.target.value });
								}}
							/>
						</div>
						<div>
							<label className="block text-sm font-medium text-gray-500">Password: </label>
							<input
								required
								className="mt-1 w-full rounded-md bg-gray-600 p-2"
								type="password"
								value={this.state.password}
								onChange={event => {
									this.setState({ password: event.target.value });
								}}
							/>
						</div>
						<div className="mb-7">
							<label className="block text-sm font-medium text-gray-500">Confirm password: </label>
							<input
								required
								className="mt-1 w-full rounded-md bg-gray-600 p-2"
								type="password"
								value={this.state.confirmedPassword}
								onChange={event => {
									this.setState({ confirmedPassword: event.target.value });
								}}
							/>
						</div>
						<button
							type="submit"
							className="w-full rounded-md bg-blue-600 p-2 text-white hover:bg-blue-700 focus:outline-none focus:ring-2 focus:ring-blue-500"
						>
							Register
						</button>
					</form>
					<div className="text-center text-sm text-gray-500">
						<span>
							Already have an account?{' '}
							<a href="/login" className="text-blue-600 hover:underline">
								Sign in
							</a>
						</span>
					</div>
				</div>
			</div>
		);
	}
}

export default navHOC(RegisterPage);
