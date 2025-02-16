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
			<div className="flex min-h-screen items-center justify-center bg-gray-100">
				<div className="w-full max-w-md space-y-6 rounded-lg bg-white p-8 shadow-lg">
					<h1 className="text-center text-2xl font-semibold text-gray-900">Sign in</h1>
					<form
						className="space-y-4"
						onSubmit={event => {
							event.preventDefault();
							this.loginHandler();
						}}
					>
						<div>
							<label className="block text-sm font-medium text-gray-700">Login: </label>
							<input
								required
								className="mt-1 w-full rounded-md border border-gray-300 p-2 focus:border-blue-500 focus:ring-blue-500"
								type="text"
								value={this.state.username}
								onInput={e => {
									this.setState({ username: e.currentTarget.value });
								}}
							/>
						</div>
						<div>
							<label className="block text-sm font-medium text-gray-700">Password: </label>
							<input
								required
								className="mt-1 w-full rounded-md border border-gray-300 p-2 focus:border-blue-500 focus:ring-blue-500"
								type="password"
								value={this.state.password}
								onInput={e => {
									this.setState({ password: e.currentTarget.value });
								}}
							/>
						</div>
						<button
							type="submit"
							className="w-full rounded-md bg-blue-600 p-2 text-white hover:bg-blue-700 focus:outline-none focus:ring-2 focus:ring-blue-500"
						>
							Login
						</button>
					</form>
					<div className="text-center text-sm text-gray-600">
						<span>
							Don&apos;t have an account?{' '}
							<a href="/register" className="text-blue-600 hover:underline">
								Sign up
							</a>
						</span>
					</div>
				</div>
			</div>
		);
	}
}

export default navHOC(LoginPage);
