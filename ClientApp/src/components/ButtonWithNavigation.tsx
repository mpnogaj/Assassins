import { useNavigate } from 'react-router';

type Props = {
	isButton: boolean;
	buttonText: string;
	onClick: () => Promise<boolean>;
	destination: string;
};

const ButtonWithNavigation = (props: Props) => {
	const navigate = useNavigate();
	const onClick = async () => {
		const shouldNavigate = await props.onClick();
		if (shouldNavigate) {
			navigate(props.destination, { replace: true });
		}
	};

	if (props.isButton) {
		return (
			<button type="submit" className="btn btn-primary mt-3" onClick={() => onClick()}>
				{props.buttonText}
			</button>
		);
	} else {
		return (
			<a className="btn btn-primary mt-3" onClick={() => onClick()}>
				{props.buttonText}
			</a>
		);
	}
};

export default ButtonWithNavigation;
