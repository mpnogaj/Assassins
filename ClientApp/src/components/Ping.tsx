const Ping = () => {
	fetch('http://localhost:5001/api/ping').then(res => res.json().then(json => console.log(json)));
	return <h2>Sending ping</h2>;
};

export default Ping;
