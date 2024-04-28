import './App.css';

function App() {
  return (
    <div className="App">
      <header className="App-header">
        <h1>Welcome to GymPro!</h1>
        <p>Your one-stop portal for managing gym operations and memberships.</p>
        <a
          className="App-link"
          href="#features"
        >
          Explore Features
        </a>
        <a
          className="App-link"
          href="#contact"
        >
          Contact Us
        </a>
      </header>

      <section id="features" className="App-section">
        <h2>Features</h2>
        <ul>
          <li>Membership Management</li>
          <li>Class Scheduling</li>
          <li>Personal Trainer Bookings</li>
          <li>Gym Equipment Tracking</li>
        </ul>
      </section>

      <section id="contact" className="App-section">
        <h2>Contact Us</h2>
        <p>Got questions? We'd love to hear from you! Reach out to us at:</p>
        <p>Email: support@gympro.com</p>
        <p>Phone: 123-456-7890</p>
      </section>

      <footer className="App-footer">
        <p>&copy; 2024 GymPro. All rights reserved.</p>
      </footer>
    </div>
  );
}

export default App;
