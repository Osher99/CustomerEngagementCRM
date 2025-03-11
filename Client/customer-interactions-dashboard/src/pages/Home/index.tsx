import Header from "../../components/Header";
import Footer from "../../components/Footer";
import InteractionsDashboard from "../../containers/InteractionsDashboard";

const Home = () => {  
    return (
      <div className="w-full flex flex-col home">
        <Header />
        <main className="flex-1 flex flex-col items-center justify-center bg-gray-100 p-4">
          <div className="w-full flex justify-center">
            <InteractionsDashboard />
          </div>
        </main>
        <Footer />
      </div>
    );
  };

export default Home;