import { empty } from '@/types/other';
import React from 'react';
import LoaderComponent from '../LoaderComponent';
import FetchErrorComponent from '../FetchErrorComponent';

export type FetchableComponentSpecialProps<T> = {
	dataFetcher: () => Promise<T | undefined>;
};

export type FetchableComponentProps<T, Y> = Y extends empty
	? FetchableComponentSpecialProps<T>
	: FetchableComponentSpecialProps<T> & Y;

export type FetchableComponentState<T> = {
	isLoading: boolean;
	isError: boolean;
	data?: T;
};

// Class used by component to consume fetched data
export class FetchableComponent<T, Y = empty, S = empty> extends React.Component<
	{ data: T; refetch: () => void } & Omit<Readonly<FetchableComponentProps<T, Y>>, 'dataFetcher'>,
	S
> {}

export function useFetchableComponent<T, Y = empty, S = empty>(
	Comp: typeof FetchableComponent<T, Y, S>
) {
	return class extends React.Component<FetchableComponentProps<T, Y>, FetchableComponentState<T>> {
		constructor(props: FetchableComponentProps<T, Y>) {
			super(props);

			this.state = {
				isLoading: true,
				isError: false,
				data: undefined
			};
		}

		onError = () => {
			this.setState({ ...this.state, isLoading: false, isError: true });
		};

		onSuccess = (data: T) => {
			this.setState({ ...this.state, isLoading: false, isError: false, data: data });
		};

		fetchData = () => {
			this.props
				.dataFetcher()
				.then(data => {
					if (data != undefined) {
						this.onSuccess(data);
					} else {
						this.onError();
					}
				})
				.catch(this.onError);
		};

		componentDidMount(): void {
			this.fetchData();
		}

		render() {
			const { dataFetcher, ...restProps } = this.props;
			// eslint-disable-next-line @typescript-eslint/no-unused-vars
			const _ = dataFetcher;

			if (this.state.isLoading) {
				return <LoaderComponent />;
			}

			if (this.state.isError || this.state.data == undefined) {
				return <FetchErrorComponent />;
			} else {
				return <Comp data={this.state.data} {...restProps} refetch={this.fetchData} />;
			}
		}
	};
}
