import { Routes, Route, Navigate } from 'react-router-dom';
import { useAuth } from './context/AuthContext.jsx';
import { ROLES } from './utils/constants';
import ProtectedRoute from './routes/ProtectedRoute.jsx';
import DashboardLayout from './components/layout/DashboardLayout.jsx';
import Settings from './pages/Settings.jsx';

import AuthPage from './pages/auth/AuthPage.jsx';
import NotFound from './pages/NotFound.jsx';

import ManagerDashboard from './pages/manager/Dashboard.jsx';
import ManagerTickets from './pages/manager/Tickets.jsx';
import ManagerTicketDetails from './pages/manager/TicketDetails.jsx';
import ManagerEmployees from './pages/manager/Employees.jsx';
import ManagerClients from './pages/manager/Clients.jsx';
import ManagerProducts from './pages/manager/Products.jsx';

import MyTickets from './pages/client/MyTickets.jsx';
import CreateTicket from './pages/client/CreateTicket.jsx';
import EditTicket from './pages/client/EditTicket.jsx';
import ClientTicketDetail from './pages/client/TicketDetail.jsx';

import AssignedTickets from './pages/employee/AssignedTickets.jsx';
import EmployeeTicketWork from './pages/employee/TicketWork.jsx';

function RoleHome() {
  const { user } = useAuth();
  if (user?.role === ROLES.MANAGER) return <Navigate to="/manager" replace />;
  if (user?.role === ROLES.EMPLOYEE) return <Navigate to="/employee" replace />;
  if (user?.role === ROLES.CLIENT) return <Navigate to="/client" replace />;
  return <Navigate to="/login" replace />;
}

export default function App() {
  return (
    <Routes>
      <Route path="/login" element={<AuthPage />} />

      <Route
        element={
          <ProtectedRoute>
            <DashboardLayout />
          </ProtectedRoute>
        }
      >
        <Route path="/" element={<RoleHome />} />
        <Route path="/settings" element={<ProtectedRoute><Settings /></ProtectedRoute>} />

        {/* Manager */}
        <Route path="/manager" element={<ProtectedRoute roles={[ROLES.MANAGER]}><ManagerDashboard /></ProtectedRoute>} />
        <Route path="/manager/tickets" element={<ProtectedRoute roles={[ROLES.MANAGER]}><ManagerTickets /></ProtectedRoute>} />
        <Route path="/manager/tickets/:id" element={<ProtectedRoute roles={[ROLES.MANAGER]}><ManagerTicketDetails /></ProtectedRoute>} />
        <Route path="/manager/employees" element={<ProtectedRoute roles={[ROLES.MANAGER]}><ManagerEmployees /></ProtectedRoute>} />
        <Route path="/manager/clients" element={<ProtectedRoute roles={[ROLES.MANAGER]}><ManagerClients /></ProtectedRoute>} />
        <Route path="/manager/products" element={<ProtectedRoute roles={[ROLES.MANAGER]}><ManagerProducts /></ProtectedRoute>} />

        {/* Client */}
        <Route path="/client" element={<ProtectedRoute roles={[ROLES.CLIENT]}><MyTickets /></ProtectedRoute>} />
        <Route path="/client/new" element={<ProtectedRoute roles={[ROLES.CLIENT]}><CreateTicket /></ProtectedRoute>} />
        <Route path="/client/tickets/:id" element={<ProtectedRoute roles={[ROLES.CLIENT]}><ClientTicketDetail /></ProtectedRoute>} />
        <Route path="/client/tickets/:id/edit" element={<ProtectedRoute roles={[ROLES.CLIENT]}><EditTicket /></ProtectedRoute>} />

        {/* Employee */}
        <Route path="/employee" element={<ProtectedRoute roles={[ROLES.EMPLOYEE]}><AssignedTickets /></ProtectedRoute>} />
        <Route path="/employee/tickets/:id" element={<ProtectedRoute roles={[ROLES.EMPLOYEE]}><EmployeeTicketWork /></ProtectedRoute>} />
      </Route>

      <Route path="*" element={<NotFound />} />
    </Routes>
  );
}
